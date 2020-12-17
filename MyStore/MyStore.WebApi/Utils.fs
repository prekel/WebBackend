module MyStore.WebApi.Utils

open System

let nullableLimitStartToSkipTake (start: Nullable<int>, limit: Nullable<int>) =
    let nskip =
        match Option.ofNullable (start) with
        | Some (x) -> x
        | None -> 0

    let ntake =
        match Option.ofNullable (limit) with
        | Some (x) -> x
        | None -> Int32.MaxValue

    (nskip, ntake)
