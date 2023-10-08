#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Sleep to allow SQL Server time to start up
sleep 30

# Restore the database
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -Q "RESTORE DATABASE pos_test_db FROM DISK='/var/opt/mssql/backup/pos_test_db.bak' WITH MOVE 'posBackup-2023-5-7-17-44 (2)_Data' TO '/var/opt/mssql/data/pos_test_db.mdf', MOVE 'posBackup-2023-5-7-17-44 (2)_Log' TO '/var/opt/mssql/data/pos_test_db.ldf'"

# Keep the container running after the script finishes
tail -f /dev/null