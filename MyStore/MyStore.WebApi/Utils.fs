module MyStore.WebApi.Utils

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc

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

module ActionResult =
    let ofAsync (res: Async<IActionResult>) = res |> Async.StartAsTask

    let ofAsyncT (res: Async<ActionResult<'T>>) = res |> Async.StartAsTask

    let ofAsyncTA (n: ActionResult -> ActionResult<'T>) (res: Async<IActionResult>) =
        async {
            let! t = res
            return downcast t |> n
        }
        |> Async.StartAsTask
