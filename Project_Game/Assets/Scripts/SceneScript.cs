using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Net;
using System.Net.Mail;

public class SceneScript : MonoBehaviour
{
    public static string fileNameFormat = "Data_{0}.csv";
    public static string fileName;
    public static string folder = "keystrokeData";
    public static string dataPath;
    public int fileNameCounter = 2;

    void Start()
    {
        dataPath = Application.dataPath + "/../" + folder;

        //Creates the folder/directory called keystrokeData if it doesnt exist yet
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        fileName = string.Format(dataPath + "/" + fileNameFormat, "1");                     //Sets the first filename to have a value of "1" added to indicate first data file for the given user.

        while (File.Exists(fileName))                                                       //On proper implementation, the filename should be the unique user id.
        {
            fileName = string.Format(dataPath + "/" + fileNameFormat, fileNameCounter++);   //Adds a value at the end of the filename which will create a new file for a new game.
        }
    }

    //Changes scene from Home Scene to Game Scene
    public void homeSceneToGameScene()
    {
        SceneManager.LoadScene(1);
    }

    //Changes scene from End Scene to Home Scene
    public void endSceneToHomeScene()
    {
        SceneManager.LoadScene(0);
    }

    //Changes scene from Game Scene to End Scene
    public static void gameSceneToEndScene()
    {
        if (Score.tilesFilled == 49)
        {
            SceneManager.LoadScene(2);
            sendEmail(fileName);
        }
    }

    //Changes scene from End Scene to Game Scene
    public void endSceneToGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public static void sendEmail(string file)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress("emailforproject21@gmail.com");     //Email address that will send the email
        mail.To.Add("emailforproject21@gmail.com");                     //Email address that will receive the email
        mail.Subject = "Keystroke Data";                                     //The subject of the email
        mail.Body = "CSV file attached";                             //The body of the email

        System.Net.Mail.Attachment attachment;
        attachment = new System.Net.Mail.Attachment(file);              //This is the file path for the data file to be attached
        mail.Attachments.Add(attachment);                               //Adds the attachment file

        SmtpServer.Port = 587;
        SmtpServer.Credentials = new System.Net.NetworkCredential("emailforproject21@gmail.com", "Useridentification1");
        SmtpServer.EnableSsl = true;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

        SmtpServer.Send(mail);

    }
}
