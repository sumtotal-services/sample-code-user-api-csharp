<p align="center" width="100%">
    <img src="https://marketplace.sumtotalsystems.com/content/images/vendor/SumTotal_logo.png"> 
</p>

# Introduction
This sample code base is intended to demonstrate the authentication and basic functionality of Sumtotal’s User related Rest API. It utilizes SumTotal’s OAuth 2.0 authentication to obtain an access token via User Authorization or Client Credentials (B2B) 
## Prerequisites
- SumTotal Admin account (Permission level tied to OAuth Client Configuration)
- [Visual Studio](https://visualstudio.microsoft.com/vs/)
- [Postman](https://www.getpostman.com)
- [Fiddler](https://www.telerik.com/fiddler)
## Swagger
- https://{site-url}/apis/swagger/ui/index
- https://{site-url}/apis/documentation
## Authentication Types
### [Authorization Code](https://www.oauth.com/oauth2-servers/server-side-apps/authorization-code/)
 The authorization code is a temporary code that the client will exchange for an access token. The code itself is obtained from the authorization server where the user gets a chance to see what the information the client is requesting, and approve or deny the request.
### [Client Credentials](https://oauth.net/2/grant-types/client-credentials/)
The Client Credentials grant type is used by clients to obtain an access token outside of the context of a user. This is typically used by clients to access resources about themselves rather than to access a user's resources.
## Functionality
1. Authorization Code
    - Create User (POST > apis/api/v1/users)
    - Get Users (GET > apis/api/v1/users)
2. Client Credentials (B2B)
    - Get Users (GET > apis/api/v1/users)
## Setup Guide
1. OAuth Configuration 
    - Login in as an Admin
    - Administration > Common Objects > Configuration > OAuth Configuration
    - Click 'Add' to create a new OAuth Client
    - Enter desired Client ID
    - PKCE disabled (Enabling this will block API calls)
    - Enter secure Client Secret
    - Select desired scopes 
        - allapis (Access SumTotal Rest APIs)
        - odatapis (Access to OData APIs)
        - offline_access (Responsible for granting a refresh token)
    - Add a redirect URL you wish to use (Used for Authorization Code Grant Type)
    - Submit
## Code Structure
1. Program.cs
    - The start and end of your C# console application.
    - The Main method is where you are able to create objects and execute methods throughout your solution.
    - Reads weather or not you want to use Client Credentials or Authorization code authentication, this is indicated by the 'b2b' key (Boolean) within your App.Config.
    - Contains the relevant functions required to obtain access tokens & refresh tokens.
2. B2BGrantOauth.cs
    - Reads configuration settings from App.Config.
    - Contains methods relating to obtaining an Access Token via the Client Credential grant type.
3. CodeGrantOauth.cs
    - Responsible for rendering a form in order to load SumTotal login screen.
        - From here a user should have login details to proceed.
    - Contains methods relating to obtaining an Access Token via the Authorization Code grant type.
    - Contains methods relating to obtaining a Refresh Token.
4. UserOperations.cs
    - Contains methods relating to user operations.
        - Create User (Authorization Code).
        - Get Users (Authorization Code & Client Credentials).
    - Creates a user object that reads from App.Config.
5. UserObject.cs
    - User model needed for body (POST).
6. App.Config
    - Configuration needed in order to successfully execute the program.
```sh
  <appSettings>
    <!--Enter the client id from the portal.-->
    <add key="ClientId" value="b2b_oidc" />
    <!--Enter the client secret from the portal-->
    <add key="ClientSecret" value="b2b_secret"/>
    <!--Default scope is all-->
    <add key="Scope" value="allapis" />
    <!--Enter the URL for the Service-->
    <add key="Environmnet" value="https://au04sales.sumtotaldevelopment.net" />
    <!--Enter the redirect uri for service-->
    <add key="RedirectUri" value="https://test.sumtotal.com/oidc"/>
    <!--User Object-->
    <add key="loginName" value="sampleAPP6"/>
    <add key="userId" value=""/>
    <add key="password" value="password123"/>
    <add key="domainId" value="-1"/>
    <add key="userTypeId" value="3"/>
    <add key="usersecurityRoleId" value="-100"/>
    <add key="firstName" value="sumtotal"/>
    <add key="lastName" value="demo"/>
    <add key="email" value="testingemail@sumt.com"/>
    <add key="languageId" value="1"/>
    <add key="timeZoneId" value="8"/>
    <add key="status" value="1"/>
    <!--Indicate if you want a b2b call-->
    <add key="b2b" value="true"/>
  </appSettings>
```
## Example Requests & Responses
##### AUTHENTICATION REQUEST
    - POST https://{site-url}/apisecurity/connect/token HTTP/1.1
        Accept: application/json
        Content-Type: application/x-www-form-urlencoded
        Host: {site-url}
        Content-Length: 73
        Expect: 100-continue
        Connection: Keep-Alive
        
        client_id=b2b_oidc&client_secret=b2b_secret&grant_type=client_credentials
        
##### AUTHENTICATION RESPONSE
    {"access_token":"eyJhbGciOiJSUzI1NiIsImtpZCI6IkEwQjVCMUFCMTUzMjI1MzRDNUIxQUU3QTdEMjZDRkI3NDYzNTIwMzMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJvTFd4cXhVeUpUVEZzYTU2ZlNiUHQwWTFJRE0ifQ.eyJuYmYiOjE1NzExMzU3ODgsImV4cCI6MTU3MTE0Mjk4OCwiaXNzIjoiaHR0cHM6Ly9hdTA0c2FsZXMuc3VtdG90YWxkZXZlbG9wbWVudC5uZXQvYXBpc2VjdXJpdHkiLCJhdWQiOlsiaHR0cHM6Ly9hdTA0c2FsZXMuc3VtdG90YWxkZXZlbG9wbWVudC5uZXQvYXBpc2VjdXJpdHkvcmVzb3VyY2VzIiwiZXh0YXBpcyJdLCJjbGllbnRfaWQiOiJiMmJfb2lkYyIsImJyb2tlcnNlc3Npb24iOiJkZjZhMzI5Zi02MjRkLTRkN2ItODBkYy1kNDdhM2U1ODVkYzIiLCJzY29wZSI6WyJhbGxhcGlzIl19.VKurjJBPar7K-KjExPJhInA9T5aIpm6NZyjVCaLLN5Dt4QJkQZTZq7p0EhEfzshtqVck2GSba-pxLNkPLbeONkBKTcYQGRKUgzlk787NPnn4_fSHCOxy-LDykIbv6G_zWcT3RW9_DE4ap5t2tmTPPgEHi3huYx_YabYL4WSpSslbs7tttIi1qI2m9NpN3apsT8uMT7izr5PbmrHGGWPhBI-lmwjx2l2Y8mh62ErPm281VSYVSrTkRPPSQHrkySLskYYiGXy0zUZuIa5abveTnTqFH9uxWL1Nt-wuC4AgRhacJTcmdaBynN8mguvQaL64fcNTt1yl9Tnf2T6XFDKogQ","expires_in":7200,"token_type":"Bearer","scope":"allapis"}
    
##### CREATE USER REQUEST
    POST https://{site-url}/apis/api/v1/users HTTP/1.1
    
    Authorization: Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkEwQjVCMUFCMTUzMjI1MzRDNUIxQUU3QTdEMjZDRkI3NDYzNTIwMzMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJvTFd4cXhVeUpUVEZzYTU2ZlNiUHQwWTFJRE0ifQ.eyJuYmYiOjE1NzA4NzUxNTAsImV4cCI6MTYxNDA3NTE1MCwiaXNzIjoiaHR0cHM6Ly9hdTEwaW50LnN1bXRvdGFsZGV2ZWxvcG1lbnQubmV0L2FwaXNlY3VyaXR5IiwiYXVkIjpbImh0dHBzOi8vYXUxMGludC5zdW10b3RhbGRldmVsb3BtZW50Lm5ldC9hcGlzZWN1cml0eS9yZXNvdXJjZXMiLCJleHRhcGlzIl0sImNsaWVudF9pZCI6InN1bXRvdGFsX29pZGNhIiwic3ViIjoiYWRtaW5pc3RyYXRvciIsImF1dGhfdGltZSI6MTU3MDg3NTE0NiwiaWRwIjoibG9jYWwiLCJuYW1lIjoiYWRtaW5pc3RyYXRvciIsInVzZXJuYW1lIjoiYWRtaW5pc3RyYXRvciIsInJvbGUiOiJQb3J0YWwgVXNlciIsInRlbmFudCI6IkFVMTBfSU5UX1RFU1QiLCJicm9rZXJzZXNzaW9uIjoiYzJkZjYyMTBmNDBkNGNlNTljMGUwZmYyZjRmOTE2MzkiLCJjdWx0dXJlIjoiZW4tVVMiLCJsYW5ndWFnZSI6ImVuLXVzIiwiZGF0ZWZvcm1hdCI6Ik1NL2RkL3l5eXkiLCJ0aW1lZm9ybWF0IjoiaGg6bW0gYSIsInVzZXJpZCI6IjEiLCJwZXJzb25wayI6Ii0xIiwiZ3Vlc3RhY2NvdW50IjoiMCIsInVzZXJ0aW1lem9uZWlkIjoiQW1lcmljYS9OZXdfWW9yayIsInByb3Blcm5hbWUiOiJTeXN0ZW0rQWRtaW4iLCJzY29wZSI6WyJwcm9maWxlIiwib3BlbmlkIiwiYWxsYXBpcyJdLCJhbXIiOlsicHdkIl19.Xfy4v7L9jLSyghS3NBjYC12h1goc-Sv12KSAxguVr9b_jOQezATPatbr8l4pS8iQXRyNqnu5BIdafKd7FzLk_tjYkSpSdDhjDdhUY6R3IZvBJDIzqdablVCBzBi2ZQPUwwiJn-RBI2J289VmEY9YLh7zy7QrD53aQ3-1NzUmj1BQdJkyFXtQgqOCDtK9ocSQ4voL7od-B8_8oyxtqWpteXVV6r4gqos8KkJc9-jh2IHGRAnMayCZuL7oSTcoDhYDWmLUiSatFI2UUmHbFGYU53ig6vSaswideIe8mr-qyIbVS9gaXf7FGKoHjD0wXK6P4qXGo4RpevbS9bGNeak9hg
    
    Content-Type: application/json
    Host: {site-url}
    Content-Length: 216
    Expect: 100-continue
    Connection: Keep-Alive
    {"loginName":"sampleAPP8","userId":"","password":"p","domainId":-1,"userTypeId":3,"usersecurityRoleId":-100,"firstName":"TEST","lastName":"TEST","email":"test8@testing.com","languageId":1,"timeZoneId":8,"status":"1"}
    
##### CREATE USER RESPONSE
   
    {
        "validationMessages": [],
        "response": "User created",
        "primaryKey": 123123123,
        "status": 2
    }
    
##### GET USERS REQUEST
    
    GET https://{site-url}/apis/api/v1/users HTTP/1.1
    
    Authorization: Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IkMyNTFGQzY0REExNDkxOTgxREIxQUIzQjVGNTkwNUQxRjlBRkNEQkIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJ3bEg4Wk5vVWtaZ2RzYXM3WDFrRjBmbXZ6YnMifQ.eyJuYmYiOjE1NzExNDQ4OTcsImV4cCI6MTU3MTE0ODQ5NywiaXNzIjoiaHR0cDovL3N0cy1kZXYvYXBpc2VjdXJpdHkiLCJhdWQiOlsiaHR0cDovL3N0cy1kZXYvYXBpc2VjdXJpdHkvcmVzb3VyY2VzIiwiZXh0YXBpcyJdLCJjbGllbnRfaWQiOiJzdW10b3RhbF9wb3dlcmJpIiwic3ViIjoiYWRtaW4iLCJhdXRoX3RpbWUiOjE1NzExNDQ4OTUsImlkcCI6ImxvY2FsIiwibmFtZSI6ImFkbWluIiwidXNlcm5hbWUiOiJhZG1pbiIsInJvbGUiOiJQb3J0YWwgVXNlciIsInRlbmFudCI6InN0cy1kZXYiLCJicm9rZXJzZXNzaW9uIjoiNDk1YTY2MTY5ZmE4NDJlNGEzMmM5YjQ1M2RjYzQ3MGUiLCJjdWx0dXJlIjoiZW4tVVMiLCJsYW5ndWFnZSI6ImVuLXVzIiwiZGF0ZWZvcm1hdCI6Ik1NL2RkL3l5eXkiLCJ0aW1lZm9ybWF0IjoiaGg6bW0gYSIsInVzZXJpZCI6IjEiLCJwZXJzb25wayI6Ii0xIiwiZ3Vlc3RhY2NvdW50IjoiMCIsInVzZXJ0aW1lem9uZWlkIjoiRXVyb3BlL1p1cmljaCIsInByb3Blcm5hbWUiOiJIZWF0aGVyK1Jvc2UiLCJwaG90b3VybCI6IjE5ZTM1MzNiYWY5YTQ3YTc5YjBlNjVlZTU4NTc0ZTM4LnBuZyIsInNjb3BlIjpbInByb2ZpbGUiLCJvcGVuaWQiLCJhbGxhcGlzIl0sImFtciI6WyJwd2QiXX0.KUk9booNAjrwk84cLcVSAOGdAu5sA3duj8xCvJ1B74EbJVRWRCy_O5ZpWUIojLzOOeOXrUGJ8yiwnGOMS2Ih5x-EFERARAo3nXHBHlDxd-nbYoyG5_F3DBf-Zcat9isKd2rW8mvT0b1gJ4PQSb4PGnGG7wd3jCM2_sqelWAn1GnhIKNQPHs2K5Z6_YFT-epj_toiWYfleg0G50Kb2OPL5dtCK7_jNZskdUeGaX7ZDHwi9Px8nOabuUrGNl-NbBDRnOajm8YoyGHeR3-mUtOrX4SO78MnjtqZoV0gRfc6G6nmrXyePFr8PsBiFPR-MCH_5SBtJmzdsftvy5JfbWOdVA
    
    Content-Type: application/x-www-form-urlencoded
    Host: {site-url}
##### GET USERS RESPONSE
    {
      "pagination": {
        "offset": 0,
        "limit": 1,
        "total": 704
      },
      "data": [
        {
          "firstName": "Florian",
          "lastName": " Fleissig",
          "fullPropertyName": null,
          "userId": "100212",
          "personId": 100460,
          "emailId": "ffleissig@company.com",
          "managerUserId": "100211",
          "managerFullName": "Lena Lippmann",
          "imagePath": "98ME7GVJ2U5Q2UO0A1BA43RF.png",
          "accountStatus": "Non Self account",
          "primaryDomain": "Oesterreich",
          "primaryOrganization": "Sales DACH",
          "jobTitle": "Technology Specialist",
          "managerId": 100459,
          "primaryJob": "Account Manager",
          "managerFirstName": "Lena",
          "managerLastName": "Lippmann",
          "userName": "ffleissig",
          "createdDate": "2015-12-24T05:48:27",
          "phone": null,
          "employeeType": "Full-time",
          "active": true,
          "loginEnabled": true,
          "locked": false,
          "maskedManagerUserId": "100211",
          "maskedUserId": "100212"
        }
      ]
    }
