namespace MyStore.Domain.PublicTypes

open MyStore.Domain.SimpleTypes
open MyStore.Domain.CompoundTypes

type Product = { ProductId: ProductId
                 Name: string
                 Description: string }