# Stock Market
A project to to demonstrate how to combine angular front end with ASP.NET Core API as back end, I used also some tools and APIs:
- Background service.
- EF Core
- SMTP
- Polygo.io API
- Kendo UI

## Features
- Manage useres and make CRUD operations.
- Send formatted email to all users every 6 houres containing stock prices.
- Clean code and well documented.

## Deployment
- In command line, go to folder: __angularwithasp\angularwithasp.client__, then execute the following command:
```sh
ng build
```
- open folder: __angularwithasp\angularwithasp.client\dist\angularwithasp.client\browser__, and copy all contents to folder: __angularwithasp\angularwithasp.server\wwwroot__ replacing its contents.
- run or publish the project: _angularwithasp.server_ using visual studio or any other tool.

## License
**Apache License 2.0**
**Free Software**
