module App.Counter1

open Feliz
open Feliz.UseDeferred

let loadData =
    async {
        do! Async.Sleep 1000
        return "Hello!"
    }

[<ReactComponent>]
let Counter1 (init: int) =
    let count, setCount = React.useState init
    let data = React.useDeferred (loadData, [||])




    let data =
        match data with
        | Deferred.HasNotStartedYet -> Html.none
        | Deferred.InProgress ->
            Html.i [ prop.className [ "fa"
                                      "fa-refresh"
                                      "fa-spin"
                                      "fa-2x" ] ]
        | Deferred.Failed error -> Html.div error.Message
        | Deferred.Resolved content -> Html.h1 content

    Html.div [ Html.h1 count
               Html.button [ prop.onClick (fun _ -> setCount (count + 1))
                             prop.text "Increment" ]
               Html.button [ prop.onClick (fun _ -> setCount (count - 2))
                             prop.text "Decrement" ]
               data ]
