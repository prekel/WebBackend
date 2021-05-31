namespace MyStore.Domain.CompoundTypes

open MyStore.Domain.SimpleTypes

type CustomerName =
    { FirstName: String50
      Honorific: String50
      LastName: String50 option }

type CustomerInfo =
    { CustomerId: CustomerId
      Name: CustomerName
      Email: EmailAddress
      CurrentCartId: CartId
      CartIds: CartId list }
