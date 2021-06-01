import { Union } from "../.fable/fable-library.3.1.11/Types.js";
import { string_type, union_type, int32_type } from "../.fable/fable-library.3.1.11/Reflection.js";

export class SignalRHub_Action extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["IncrementCount", "DecrementCount"];
    }
}

export function SignalRHub_Action$reflection() {
    return union_type("MyStore.Domain.Chat.SignalRHub.Action", [], SignalRHub_Action, () => [[["Item", int32_type]], [["Item", int32_type]]]);
}

export class SignalRHub_Response extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["NewCount", "TickerCount"];
    }
}

export function SignalRHub_Response$reflection() {
    return union_type("MyStore.Domain.Chat.SignalRHub.Response", [], SignalRHub_Response, () => [[["Item", int32_type]], [["Item", string_type]]]);
}

