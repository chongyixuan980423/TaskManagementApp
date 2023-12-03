# TaskManagementApp

Set up database in SSMS(SQL Server Management Studio):
1. Download the template database backup file in the TemplateDatabase folder.
2. Restore database uding the backup file.
3. After successfully restored, you will see a database named TaskManagementDB and a table named Tasks.

Set up Task Management Application:
1. Open TaskManagementApp.sln, there will be two projects in this solution which are TaskManagementApp and TaskManagementAPI.
2. Replace SERVER with your SSMS server in appsettings.json for both projects.
   ![image](https://github.com/chongyixuan980423/TaskManagementApp/assets/77527833/629d4593-2c27-4b9c-9829-ec394034f46c)
3. Right click on solution -> Configure Startup Projects.. -> Follow below settings then click Apply and OK.
   ![image](https://github.com/chongyixuan980423/TaskManagementApp/assets/77527833/0abe39e1-4747-4293-b9ee-95b95a5fda5e)
4. Press F5 to start the programs.



