namespace MyStore.Saturn.Products

open FSharp.UMX
open MyStore.Saturn.Domain

[<CLIMutable>]
type ProductDto =
    { ProductId: int
      Name: string
      Description: string
      Price: decimal
      Rating: int }

    static member FromDomain(product: Product) =
        { ProductId = %product.ProductId
          Name = %product.Name
          Description = %product.Description
          Price = %product.Price
          Rating =
              match product.Rating with
              | Rating01 -> 1
              | Rating02 -> 2
              | Rating03 -> 3
              | Rating04 -> 4
              | Rating05 -> 5
              | Rating06 -> 6
              | Rating07 -> 7
              | Rating08 -> 8
              | Rating09 -> 9
              | Rating10 -> 10 }

    member this.ToDomain(): Product =
        { ProductId = %this.ProductId
          Name = %this.Name
          Description = %this.Description
          Price = %this.Price
          // TODO: rating
          Rating = ProductRating.Rating01 }

module Validation =
    let validate v =
        let validators =
            [ fun u ->
                if (1 > u.Rating || u.Rating > 10)
                then Some("Rating", "Rating должен быть от 1 до 10")
                else None ]

        validators
        |> List.fold (fun acc e ->
            match e v with
            | Some (k, v) -> Map.add k v acc
            | None -> acc) Map.empty
