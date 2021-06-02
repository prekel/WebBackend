module MyStore.Client.Customer

open Feliz

open MyStore.Dto.Shop
open MyStore.Domain.Shop

[<ReactComponent>]
let Customer (customerModel: CustomerDto) =
    let customer = Customer.ToDomain customerModel

    Html.div [ Html.p $"%A{customer}" ]
