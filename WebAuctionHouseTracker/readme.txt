Prepare Database!

1. Create initial migration
dotnet ef Migrations add Initial

2. After creating initial migration, To create Hypertables add this in the end of UP Method in the migration file.

migrationBuilder.Sql(
"SELECT create_hypertable( '\"CommodityAuctions\"', 'TimeStamp');\n" +
"CREATE INDEX ix_commodity_timestamp ON \"CommodityAuctions\" (\"ItemId\", \"TimeStamp\" DESC)"
);

migrationBuilder.Sql(
"SELECT create_hypertable( '\"NonCommodityAuctions\"', 'TimeStamp');\n" +
"CREATE INDEX ix_noncommodity_timestamp ON \"NonCommodityAuctions\" (\"ItemId\", \"TimeStamp\", \"RealmId\" DESC)"
);

3. Update Database
dotnet ef database update

4. If you don't know connected realm id of your realm, leave "realms" field empty.
This way it'll display all available connected realms with their ids on the main page.

5. docker-compose up


===DEBUG===
IF “Npgsql: 42883: function create_hypertable(…) does not exist” appears send SQL query:
CREATE EXTENSION IF NOT EXISTS timescaledb CASCADE;