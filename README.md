# progress-visualiser-api
An API to support basic CRUD functionality and data processing for the [progress-visualiser-frontend](https://github.com/yalshaeyr/progress-visualiser-frontend) repo


## Configuration

### Deployment
Follow the instructions listed in the [azure/webapps-deploy repo](https://github.com/Azure/webapps-deploy?tab=readme-ov-file#configure-deployment-credentials-1)

### Permissions
In the target SQL database (the database identified in the `AZURE_SQL_CONNECTIONSTRING` application string), run the commands below. Replace `app-pvapi-test` with the Azure App Service identifier. Make sure to turn on the App Service's system-assigned managed identity.

```SQL
CREATE USER [app-pvapi-test] FROM EXTERNAL PROVIDER;

GRANT SELECT, DELETE, INSERT, ALTER, UPDATE ON [PV].[Metrics] TO [app-pvapi-test]
GRANT SELECT, DELETE, INSERT, ALTER, UPDATE ON [PV].[MetricData] TO [app-pvapi-test]
GRANT SELECT, DELETE, INSERT, ALTER, UPDATE ON [PV].[Users] TO [app-pvapi-test]
```