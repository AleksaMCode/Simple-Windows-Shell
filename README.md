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
* create - create a directory or a file
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
Detailed explanation of the individual commands. All commands are based on the real Linux commands and they are simplified.
### WHERE
#### NAME 
where - print the name of the current/working directory
#### SYNOPSIS
**where**
#### DESCRIPTION
Print the full filename of the current working directory.
#### LINUX COUNTERPART
Command **pwd** is the Linux counterpart.

### GO
#### NAME 
go - change the working directory
#### SYNOPSIS
**go** [*path*]
**go** ..
#### DESCRIPTION
Change the working directory of the current "shell execution environment".
1. If no *path* operand is given, error message "*The command or its signature is wrong.*" will be displayed.
2. If the *path* operand is dot-dot, the current path will be changed to the previous subdirectory.
#### LINUX COUNTERPART
Command **cd** is the Linux counterpart.

### CREATE
#### NAME 
create - create a directory or a file
#### SYNOPSIS
**create** [*-d*] *path*
#### DESCRIPTION
Create the directorie or a file specified by the operand. If no option is used, command *create* will create a new file. If no *path* operand is given, error message "*The command or its signature is wrong.*" will be displayed.
#### OPTIONS
* **-d** *path* Create a directory at a speciefied path.
#### LINUX COUNTERPART
Command **mkidr**/**touch** is the Linux counterpart.

### LIST
#### NAME 
list - list directory contents 
#### SYNOPSIS
**list** [*path*]
#### DESCRIPTION
List object names (files and directories) alphabetically. If no *path* operand is given, current directory content will be displayed.
#### LINUX COUNTERPART
Command **ls** is the Linux counterpart.

### PRINT
#### NAME 
print - print txt file on the standard output 
#### SYNOPSIS
**list** [*path*]
#### DESCRIPTION
Print a file with extensions.
1. If no *path* operand is given, error message "*The command or its signature is wrong.*" will be displayed.
2. If file has no extension, error message "*File **filename** doesn't have extension information.*" or "*File doesn't have an extension.*" will be displayed.
3. If file isn't a text file, error message "*File **filename** is not a text file.*" will be displayed.
4. If file doesn't exist in the given path, error message "*File **filename** doesn't exist in path **path**.*"
#### LINUX COUNTERPART
Command **cat** is the Linux counterpart.
