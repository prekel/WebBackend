module Config

open Sql

type Config =
    { Context: Sql.dataContext
      connectionString: string }
