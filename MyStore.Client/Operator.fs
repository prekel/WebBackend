module MyStore.Client.Operator

open Feliz

open MyStore.Dto.Support
open MyStore.Domain.Support

[<ReactComponent>]
let Operator (operatorModel: OperatorDto) =
    let operator = Operator.ToDomain operatorModel

    Html.div [ Html.p $"%A{operator}" ]
