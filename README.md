# Simple Windows Shell
**SWH** is a console applications written in **C#** that emulates Linux-like command behavior on Windows.
## Usage
### Login
**LoginScreen()** function is used when signing onto a "system". To use the application user must first login. At login, user needs to provide his *Username* and *Password*. **LoginScreen()** prompts for the *Username*. The user is then prompted for a *Password*, where appropriate. Echoing is disabled to prevent revealing the *Password* and its length.

The application will check if such user exists in databese **Users.db**. If user exists and the *Password* is correct, user's "shell" is started, otherwise following is writtien "*Login incorrect*" to the console and the user is then prompted for the *Username* again. The whole proccess is made to mirror Linux-like behavior with *init* and *login* process.

### Shell
Once user has succesfully logged in, he will be in **SWH** shell. Shell will prompt user for a new command. Here is the list of the avalable commands:
1. where
2. go *path*
