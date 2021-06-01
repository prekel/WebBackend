[<AutoOpen>]
module Extensions

open System
open Fable.Core
open Fable.Core.JsInterop
open Fetch
open Thoth.Json

[<RequireQualifiedAccess>]
module StaticFile =

    /// Function that imports a static file by it's relative path.
    let inline import (path: string) : string = importDefault<string> path

[<RequireQualifiedAccess>]
module Config =
    /// Returns the value of a configured variable using its key.
    /// Retursn empty string when the value does not exist
    [<Emit("process.env[$0] ? process.env[$0] : ''")>]
    let variable (key: string) : string = jsNative

    /// Tries to find the value of the configured variable if it is defined or returns a given default value otherwise.
    let variableOrDefault (key: string) (defaultValue: string) =
        let foundValue = variable key

        if String.IsNullOrWhiteSpace foundValue then
            defaultValue
        else
            foundValue

// Stylesheet API
// let private stylehsheet = Stylesheet.load "./fancy.css"
// stylesheet.["fancy-class"] which returns a string
module Stylesheet =

    type IStylesheet =
        [<Emit "$0[$1]">]
        abstract Item : className: string -> string

    /// Loads a CSS module and makes the classes within available
    let inline load (path: string) = importDefault<IStylesheet> path

module private Nullable =
    let IntDecoder =
        Decode.object (fun get -> get.Optional.Raw Decode.int |> Option.toNullable)

    let IntEncoder (nullable: Nullable<int>) =
        Encode.option Encode.int (nullable |> Option.ofNullable)

    let DateTimeOffsetDecoder =
        Decode.object
            (fun get ->
                get.Optional.Raw Decode.datetimeOffset
                |> Option.toNullable)

    let DateTimeOffsetEncoder (nullable: Nullable<DateTimeOffset>) =
        Encode.option Encode.datetimeOffset (nullable |> Option.ofNullable)

let extra : ExtraCoders =
    Extra.empty
    |> Extra.withCustom Nullable.IntEncoder Nullable.IntDecoder
    |> Extra.withCustom Nullable.DateTimeOffsetEncoder Nullable.DateTimeOffsetDecoder

let acceptJson =
    [ HttpRequestHeaders.Accept "application/json" ]
