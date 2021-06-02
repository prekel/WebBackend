import { Record, Union } from "../.fable/fable-library.3.1.11/Types.js";
import { record_type, int32_type, string_type, union_type } from "../.fable/fable-library.3.1.11/Reflection.js";
import { Question$reflection, Answer$reflection, Ticket$reflection } from "./Support.js";

export class SignalRHub_Role extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Operator", "Customer"];
    }
}

export function SignalRHub_Role$reflection() {
    return union_type("MyStore.Domain.Chat.SignalRHub.Role", [], SignalRHub_Role, () => [[], []]);
}

export class SignalRHub_ActionType extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["JoinRoom", "Message", "LeaveRoom"];
    }
}

export function SignalRHub_ActionType$reflection() {
    return union_type("MyStore.Domain.Chat.SignalRHub.ActionType", [], SignalRHub_ActionType, () => [[], [["Item", string_type]], []]);
}

export class SignalRHub_Action extends Record {
    constructor(Role, TicketId, Action) {
        super();
        this.Role = Role;
        this.TicketId = (TicketId | 0);
        this.Action = Action;
    }
}

export function SignalRHub_Action$reflection() {
    return record_type("MyStore.Domain.Chat.SignalRHub.Action", [], SignalRHub_Action, () => [["Role", SignalRHub_Role$reflection()], ["TicketId", int32_type], ["Action", SignalRHub_ActionType$reflection()]]);
}

export class SignalRHub_Response extends Union {
    constructor(tag, ...fields) {
        super();
        this.tag = (tag | 0);
        this.fields = fields;
    }
    cases() {
        return ["Joined", "NewAnswer", "NewQuestion", "LeaveDone", "Forbidden", "NotFound"];
    }
}

export function SignalRHub_Response$reflection() {
    return union_type("MyStore.Domain.Chat.SignalRHub.Response", [], SignalRHub_Response, () => [[["Item", Ticket$reflection()]], [["Item", Answer$reflection()]], [["Item", Question$reflection()]], [["Item", int32_type]], [["Item", int32_type]], [["Item", int32_type]]]);
}

