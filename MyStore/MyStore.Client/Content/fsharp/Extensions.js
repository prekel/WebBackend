import { isNullOrWhiteSpace } from "./.fable/fable-library.3.1.11/String.js";
import { datetimeOffset, int, object } from "./.fable/Thoth.Json.4.0.0/Decode.fs.js";
import { ofNullable, toNullable } from "./.fable/fable-library.3.1.11/Option.js";
import { uncurry } from "./.fable/fable-library.3.1.11/Util.js";
import { datetimeOffset as datetimeOffset_1, option as option_1 } from "./.fable/Thoth.Json.4.0.0/Encode.fs.js";
import { newGuid } from "./.fable/fable-library.3.1.11/Guid.js";
import { add } from "./.fable/fable-library.3.1.11/Map.js";
import { empty } from "./.fable/Thoth.Json.4.0.0/Extra.fs.js";
import { ExtraCoders } from "./.fable/Thoth.Json.4.0.0/Types.fs.js";
import { Types_HttpRequestHeaders } from "./.fable/Fable.Fetch.2.1.0/Fetch.fs.js";
import { singleton } from "./.fable/fable-library.3.1.11/List.js";

export function Config_variableOrDefault(key, defaultValue) {
    const foundValue = process.env[key] ? process.env[key] : '';
    if (isNullOrWhiteSpace(foundValue)) {
        return defaultValue;
    }
    else {
        return foundValue;
    }
}

const Nullable_IntDecoder = (path) => ((v) => object((get$) => toNullable(get$.Optional.Raw(uncurry(2, int))), path, v));

function Nullable_IntEncoder(nullable) {
    return option_1((value) => value)(ofNullable(nullable));
}

const Nullable_DateTimeOffsetDecoder = (path_1) => ((v) => object((get$) => toNullable(get$.Optional.Raw((path, value) => datetimeOffset(path, value))), path_1, v));

function Nullable_DateTimeOffsetEncoder(nullable) {
    return option_1((value) => datetimeOffset_1(value))(ofNullable(nullable));
}

export const extra = (() => {
    let copyOfStruct, copyOfStruct_1;
    const extra_4 = new ExtraCoders((copyOfStruct = newGuid(), copyOfStruct), add("System.Nullable`1[System.Int32]", [(nullable) => Nullable_IntEncoder(nullable), Nullable_IntDecoder], empty.Coders));
    return new ExtraCoders((copyOfStruct_1 = newGuid(), copyOfStruct_1), add("System.Nullable`1[System.DateTimeOffset]", [(nullable_1) => Nullable_DateTimeOffsetEncoder(nullable_1), Nullable_DateTimeOffsetDecoder], extra_4.Coders));
})();

export const acceptJson = singleton(new Types_HttpRequestHeaders(0, "application/json"));

