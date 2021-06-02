import { Record } from "../../.fable/fable-library.3.1.11/Types.js";
import { array_type, record_type, string_type, class_type, int32_type } from "../../.fable/fable-library.3.1.11/Reflection.js";

export class AnswerDto extends Record {
    constructor(supportAnswerId, supportTicketId, supportOperatorId, sendTimestamp, text) {
        super();
        this.supportAnswerId = (supportAnswerId | 0);
        this.supportTicketId = (supportTicketId | 0);
        this.supportOperatorId = (supportOperatorId | 0);
        this.sendTimestamp = sendTimestamp;
        this.text = text;
    }
}

export function AnswerDto$reflection() {
    return record_type("MyStore.Dto.Support.AnswerDto", [], AnswerDto, () => [["supportAnswerId", int32_type], ["supportTicketId", int32_type], ["supportOperatorId", int32_type], ["sendTimestamp", class_type("System.DateTimeOffset")], ["text", string_type]]);
}

export class OperatorDto extends Record {
    constructor(supportOperatorId, firstName, lastName, email) {
        super();
        this.supportOperatorId = (supportOperatorId | 0);
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
    }
}

export function OperatorDto$reflection() {
    return record_type("MyStore.Dto.Support.OperatorDto", [], OperatorDto, () => [["supportOperatorId", int32_type], ["firstName", string_type], ["lastName", string_type], ["email", string_type]]);
}

export class QuestionDto extends Record {
    constructor(supportQuestionId, supportTicketId, sendTimestamp, readTimestamp, text) {
        super();
        this.supportQuestionId = (supportQuestionId | 0);
        this.supportTicketId = (supportTicketId | 0);
        this.sendTimestamp = sendTimestamp;
        this.readTimestamp = readTimestamp;
        this.text = text;
    }
}

export function QuestionDto$reflection() {
    return record_type("MyStore.Dto.Support.QuestionDto", [], QuestionDto, () => [["supportQuestionId", int32_type], ["supportTicketId", int32_type], ["sendTimestamp", class_type("System.DateTimeOffset")], ["readTimestamp", class_type("System.Nullable`1", [class_type("System.DateTimeOffset")])], ["text", string_type]]);
}

export class TicketDto extends Record {
    constructor(supportTicketId, customerId, supportOperatorId, orderId, createTimestamp) {
        super();
        this.supportTicketId = (supportTicketId | 0);
        this.customerId = (customerId | 0);
        this.supportOperatorId = (supportOperatorId | 0);
        this.orderId = orderId;
        this.createTimestamp = createTimestamp;
    }
}

export function TicketDto$reflection() {
    return record_type("MyStore.Dto.Support.TicketDto", [], TicketDto, () => [["supportTicketId", int32_type], ["customerId", int32_type], ["supportOperatorId", int32_type], ["orderId", class_type("System.Nullable`1", [int32_type])], ["createTimestamp", class_type("System.DateTimeOffset")]]);
}

export class TicketsModel extends Record {
    constructor(tickets) {
        super();
        this.tickets = tickets;
    }
}

export function TicketsModel$reflection() {
    return record_type("MyStore.Dto.Support.TicketsModel", [], TicketsModel, () => [["tickets", array_type(TicketDto$reflection())]]);
}

