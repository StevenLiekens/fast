docker pull swaggerapi/swagger-codegen-cli-v3
docker run --rm -v %CD%:/local swaggerapi/swagger-codegen-cli-v3 generate -i /local/openapi.yaml -l aspnetcore -o /local/gen --ignore-file-override=/local/.swagger-codegen-ignore
rmdir .\gen\src\IO.Swagger\Filters
rmdir .\gen\src\IO.Swagger\Properties
rmdir .\gen\src\IO.Swagger\wwwroot