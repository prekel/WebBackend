namespace Products

[<CLIMutable>]
type Product =
    { ProductId: int
      Name: string
      Description: string
      Price: decimal }

module Validation =
    let validate v =
        let validators =
            [
            //              fun u ->
//                if isNull u.ProductId
//                then Some("ProductId", "ProductId shouldn't be empty")
//                else None
            ]

        validators
        |> List.fold (fun acc e ->
            match e v with
            | Some (k, v) -> Map.add k v acc
            | None -> acc) Map.empty
