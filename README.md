
# MIUTranslate
Command line application to translate the imput from a source language to a target language into a text file to be later used.

Use case: Twitch command !command Hello how are you is sent to the .exe then saved to a .txt file. Same command takes the result from the text file and send it back to chat translated

Written in C#. Use Visual Studio to build it or download the latest release

Create a HKEY_CURRENT_USER Environement AZURE_KEY REG_SZ (Using Regedit) with your personal AZURE api key (free)

**Usage:**
- /T="ZZ": Where ZZ is the translated TO language in ISO code (https://docs.microsoft.com/en-us/azure/cognitive-services/translator/language-support)
Default is EN english
- /content="": Where you send stuff to be translated 
- /path="": Where you set the path to save the translated result in .txt form
Default is c:/temp/transdlated.txt


**TODO:**
- Add config file for default setting and api key


Message Pixel beard QC in the MixItUp Discord for any questions (https://mixitupapp.com/discord)

Thankyou to CKY for all the help
