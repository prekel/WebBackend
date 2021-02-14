module Sql

open FSharp.Data.Sql

[<Literal>]
let resolutionPath = __SOURCE_DIRECTORY__ + "/libraries"

[<Literal>]
let connectionString =
    "Host=localhost;Database=postgres;Username=postgres;Password=qwerty123"

type Sql = SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL, connectionString, ResolutionPath=resolutionPath>
