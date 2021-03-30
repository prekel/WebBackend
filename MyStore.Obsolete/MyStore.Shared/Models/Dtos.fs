module MyStore.Shared.Dtos

type CustomerDto =
    { FirstName: string
      LastName: string
      Honorific: string
      Email: string
      Password: string option}

