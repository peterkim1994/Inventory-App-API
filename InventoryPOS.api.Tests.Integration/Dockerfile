# Use the official SQL Server 2019 image as a base
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables (change SA_PASSWORD to something more secure)
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=SomeStrongPassword1!
ENV MSSQL_PID=Developer 

# Create a directory for the backup file and move to it
WORKDIR /var/opt/mssql/backup
USER root

# Copy the .bak file from the local directory into the image
COPY ./pos_test_db.bak /var/opt/mssql/backup/pos_test_db.bak

# Expose SQL Server port
EXPOSE 1433

# Copy the script to restore the database into the image
COPY ./SetupTestDatabase.sh /var/opt/mssql/backup/SetupTestDatabase.sh

#ENTRYPOINT ["var/opt/mssql/backup/SetupTestDatabase.sh"]
CMD ["sh", "SetupTestDatabase.sh"]