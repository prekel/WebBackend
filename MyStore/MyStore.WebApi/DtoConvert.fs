module MyStore.WebApi.DtoConvert

open MyStore.Data
open MyStore.Data.Entity
open MyStore.Shared.Dtos

let customerDtoFromEntity (entity: Customer) =
    { FirstName = entity.FirstName
      LastName = entity.LastName
      Honorific = entity.Honorific
      Email = entity.Email
      Password = None }

let customerEntityFromDto (dto: CustomerDto) =
    let customerEntityFromDtoWithPassword dto =
        let salt = Crypto.GenerateSaltForPassword()

        let hash =
            Crypto.ComputePasswordHash(dto.Password.Value, salt)

        Customer
            (FirstName = dto.FirstName,
             LastName = dto.LastName,
             Honorific = dto.Honorific,
             Email = dto.Email,
             PasswordHash = hash,
             PasswordSalt = salt)

    let customerEntityFromDtoWithoutPassword dto =
        Customer(FirstName = dto.FirstName, LastName = dto.LastName, Honorific = dto.Honorific, Email = dto.Email)

    match dto.Password with
    | Some _ -> customerEntityFromDtoWithPassword dto
    | None -> customerEntityFromDtoWithoutPassword dto
