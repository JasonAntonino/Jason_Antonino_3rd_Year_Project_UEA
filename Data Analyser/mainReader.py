import csv

runFirstPart = True                 #This collects the keypress sequence from all of the CSV files.
runSecondPart = True                #This calculates the similarity of the keystroke sequences of participants' first and last games
runThirdPart = True                 #This calculates the frequency of keypress made by all of the participants for all of their games


textFilename = "sequences.txt"              #This will be the text file which will store all keystroke sequence of all files
similarityFilename = "similarity_sequence.txt"
percentFilename = "similarity_percentage.txt"
frequencyFilename = "keypress_frequency.txt"
fileNumber = 1                              #This is the counter for the filename
firstGamesList = [1,7,13,19,25,31,37,43,49,55,61,67,73,79,85,91,97,103,109,115]

if runFirstPart == True:
    #Iterates through each csv files and collects the keypress sequence
    for i in range(120):
        with open("Data_" + str(fileNumber) + ".csv") as csvFile:
            csvReader = csv.reader(csvFile, delimiter = ',')
            counter = 0
            
            sequenceText = ""                   #This will store the full keypress sequence that will be stored in the new text file.
            
            for row in csvReader:
                if counter == 0:
                    counter += 1

                else:
                    sequenceText += row[3]      #This appends the keypress to the variable sequenceText

            
            textFile = open(textFilename,"a")   #Opens the text file
            textFile.write(sequenceText + "\n") #Writes keypress sequence to the text file
            textFile.close()

            if fileNumber % 6 == 0 or fileNumber in firstGamesList:
                similarityFile = open(similarityFilename, "a")
                similarityFile.write(sequenceText + "\n")
                similarityFile.close()
        
        fileNumber += 1                         #This increments along with the data file number



#============================================================================
#============================= VARIABLES USED ===============================
#============================================================================

#sequences                  =   List of all of the sequences of the first and last games of all participants (40 sequences)
#userFirstGames             =   List of all sequences from the first games (20 sequences)
#userLastGames              =   List of all sequences from the last games (20 sequences)
#firstAndLastGamePairList   =   List of first and last game pairings for each participant (20 pairs)
#firstSequence              =   Stores the sequence of the first game (used in for loop)
#lastSequence               =   Stores the sequence of the last game (used in for loop)
#shortestLength             =   Stores an integer value of the shortest length between the pair of sequences

if runSecondPart == True:
    with open('similarity_sequence.txt', 'r') as f:
        sequences = [line.strip() for line in f]

    userFirstGames = sequences[0::2]                                #This is a list which stores all the first games of all participants
    userLastGames = sequences[1::2]                                 #This is a list which stores all the last games of all participants

    firstAndLastGamePairZip = zip(userFirstGames, userLastGames)    #Combines the two lists element-wise
    firstAndLastGamePairList = list(firstAndLastGamePairZip)        #Converts the zip to a list

    #Iterates through each list in the pairing list
    loopCounter = 1
    meanValue = 0
    for alist in firstAndLastGamePairList:
        firstSequence = alist[0]                    #Sequence of the first game
        firstSequenceLength = len(firstSequence)    #Length of sequence of first game
        
        lastSequence = alist[1]                     #Sequence of the last game
        lastSequenceLength = len(lastSequence)      #Length of sequence of last game

        #Calculates the absolute difference (no negative result) between the two sequence length
        sequenceLengthDifference = abs(firstSequenceLength - lastSequenceLength)    
        
        #Identifies the shortest length of sequence between the two given sequences
        shortestLength = 0
        if firstSequenceLength < lastSequenceLength:
            shortestLength = firstSequenceLength
        else:
            shortestLength = lastSequenceLength

        #Compares the two sequences character by character and calculates the accuracy (similarity) between the two
        numberOfCorrect = 0                         #Counts the total number of matching characters on the sequence
        similarityPercentage = 0                    #Stores the percentage of similarity
        for i in range(shortestLength):
            firstSequenceChar = firstSequence[i]    #Stores current character of first sequence
            lastSequenceChar = lastSequence[i]      #Stores current character of last sequence

            #If the current characters are the same (MATCH)
            if firstSequenceChar == lastSequenceChar:
                numberOfCorrect += 1    #Increase the number of correct

        #Calculates the percentage of accuracy 
        similarityPercentage = (100 / firstSequenceLength) * numberOfCorrect
        similarityPercentage = round(similarityPercentage, 3)
        meanValue += similarityPercentage
        string = "Participant " + str(loopCounter) + " Accuracy : " + str(similarityPercentage) + "%"
        
        #Stores the percentages on a text file
        percentFile = open(percentFilename,"a")     #Opens the text file
        percentFile.write(string + "\n")            #Writes keypress sequence to the text file
        percentFile.close()

        loopCounter += 1

    #Calculates the mean value of all the percentages and stores result value into the similarity_percentage text file.
    meanValue = round((meanValue / 20), 3)
    meanValueString = "Mean Value of Percentages : " + str(meanValue) + "%"

    percentFile = open(percentFilename,"a")   #Opens the text file
    percentFile.write("\n" + meanValueString + "\n") #Writes keypress sequence to the text file
    percentFile.close()


if runThirdPart == True:
    with open('sequences.txt', 'r') as f:
        allSequences = [line.strip() for line in f]

    #Finds the length of the longest sequence
    longestSequence = 0
    for sequence in allSequences:
        if len(sequence) > longestSequence:
            longestSequence = len(sequence)

    #Initialises list of lists
    frequencyList = [[0,0,0,0] for x in range(longestSequence)]

    for sequence in allSequences:                       #Iterates through all sequences
        aCounter = 0

        for character in sequence:                      #Iterates through all characters per sequence
            characterFrequencyList = [0,0,0,0]

            #Increments the correct index within the list depending on the keypress
            if character == 'W':
                characterFrequencyList[0] += 1

            if character == 'A':
                characterFrequencyList[1] += 1

            if character == 'S':
                characterFrequencyList[2] += 1

            if character == 'D':
                characterFrequencyList[3] += 1
            
            #Adds the current character frequencies to the main list
            for i in range(4):
                frequencyList[aCounter][i] = frequencyList[aCounter][i] + characterFrequencyList[i]
            
            aCounter += 1

    frequencyFile = open(frequencyFilename,"a")     #Opens the text file
    frequencyFile.write(str(frequencyList))            #Writes keypress frequency to the text file
    frequencyFile.close()
    print(frequencyList)

