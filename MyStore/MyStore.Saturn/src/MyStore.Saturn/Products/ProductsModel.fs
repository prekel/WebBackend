namespace Products

[<CLIMutable>]
type Product =
    { ProductId: int
      Name: string
      Description: string
      Price: decimal
      Rating: int }

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
