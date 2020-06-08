# Simple Windows Shell
**SWH** is a console applications written in **C#** that emulates Linux-like command behavior on Windows.
## Usage
### Login
**LoginScreen()** function is used when signing onto a "system". To use the application user must first login. At login, user needs to provide his *Username* and *Password*. **LoginScreen()** prompts for the *Username*. The user is then prompted for a *Password*, where appropriate. Echoing is disabled to prevent revealing the *Password* and its length.

The application will check if such user exists in databese **Users.db**. If user exists and the *Password* is correct, user's "shell" is started, otherwise following is writtien "*Login incorrect*" to the console and the user is then prompted for the *Username* again. The whole proccess is made to mirror Linux-like behavior with *init* and *login* process.

### Shell
Once user has succesfully logged in, he will be in **SWH** shell. Shell will prompt user for a new command. Commands *addUser* and *rmUser* can only be used by **root** user. Here is the list of the available commands:
* where - print the name of the current/working directory
* go - change the working directory
  * go *path*
  * go ..
* create - create a directory
  * create [*-d*] *path*
* list - list directory contents
  * list [*path*]
* print - print txt file on the standard output
  * print *path*
* find - print lines that match pattern with a line number
  * find "*pattern*" path 
* findDat - search for files in a directory hierarchy
  * findDat *filename* *path*
* clear - reset the terminal
* alias - customize "bash prompt"
  * alias
  * alias *newaliasname*
* exit - cause user's shell termination
* logout - logout and return to the "*login* process" (**LoginScreen()**)
* addUser - create a new user
* rmUser - delete a user account and related files

### Database
The database contains 3 users that have equivalent *Username* and *Password*.
| N. | Username | Password |
| ------- | ------- | ------- |
|1.       | aleksa  | aleksa  |
|2.       | majkic  | majkic  |
|3.       | root    | root    |

Also, here's the property query for my SQLite database:
~~~~
CREATE TABLE Users (
    Id                INTEGER   PRIMARY KEY AUTOINCREMENT
                                UNIQUE,
    Username          CHAR (20) NOT NULL
                                UNIQUE,
    Salt              BLOB,
    PassHash          BLOB,
    LastLogin         CHAR (50)
);
~~~~

## Commands
