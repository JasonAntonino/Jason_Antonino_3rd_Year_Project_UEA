using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class Email : MonoBehaviour
{
    public static string emailString;

    // Start is called before the first frame update
    void Start()
    {
        var input = gameObject.GetComponent<InputField>();
        input.onEndEdit.AddListener(SubmitEmail);
    }

    private void SubmitEmail(string email)
    {
        StringBuilder sb = new StringBuilder();
        var sha256 = System.Security.Cryptography.SHA256.Create();
        byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(email.Trim().ToLower());
        byte[] hash = sha256.ComputeHash(inputBytes);

        foreach (Byte b in hash)
            sb.Append(b.ToString("x2"));

        emailString = sb.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
