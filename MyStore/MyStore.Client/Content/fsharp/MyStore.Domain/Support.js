import { Record } from "../.fable/fable-library.3.1.11/Types.js";
import { class_type, record_type, option_type, string_type, int32_type } from "../.fable/fable-library.3.1.11/Reflection.js";
import { toNullable, ofNullable, map } from "../.fable/fable-library.3.1.11/Option.js";
import { TicketDto, QuestionDto, AnswerDto, OperatorDto } from "./Dto/Support.js";

export class Operator extends Record {
    constructor(SupportOperatorId, FirstName, LastName, Email, UserId) {
        super();
        this.SupportOperatorId = (SupportOperatorId | 0);
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.UserId = UserId;
    }
}

export function Operator$reflection() {
    return record_type("MyStore.Domain.Support.Operator", [], Operator, () => [["SupportOperatorId", int32_type], ["FirstName", string_type], ["LastName", string_type], ["Email", string_type], ["UserId", option_type(string_type)]]);
}

export function Operator_ToDomain_Z2DE961B1(dto) {
    return new Operator(dto.supportOperatorId, dto.firstName, dto.lastName, dto.email, map((x_6) => x_6, ofNullable(dto.userId)));
}

export function Operator__FromDomain(this$) {
    return new OperatorDto(this$.SupportOperatorId, this$.FirstName, this$.LastName, this$.Email, toNullable(map((x_6) => x_6, this$.UserId)));
}

export class Answer extends Record {
    constructor(SupportAnswerId, SupportTicketId, SupportOperatorId, SendTimestamp, Text$) {
        super();
        this.SupportAnswerId = (SupportAnswerId | 0);
        this.SupportTicketId = (SupportTicketId | 0);
        this.SupportOperatorId = (SupportOperatorId | 0);
        this.SendTimestamp = SendTimestamp;
        this.Text = Text$;
    }
}

export function Answer$reflection() {
    return record_type("MyStore.Domain.Support.Answer", [], Answer, () => [["SupportAnswerId", int32_type], ["SupportTicketId", int32_type], ["SupportOperatorId", int32_type], ["SendTimestamp", class_type("System.DateTimeOffset")], ["Text", string_type]]);
}

export function Answer_ToDomain_6B06E833(dto) {
    return new Answer(dto.supportAnswerId, dto.supportTicketId, dto.supportOperatorId, dto.sendTimestamp, dto.text);
}

export function Answer__FromDomain(this$) {
    return new AnswerDto(this$.SupportAnswerId, this$.SupportTicketId, this$.SupportOperatorId, this$.SendTimestamp, this$.Text);
}

export class Question extends Record {
    constructor(SupportQuestionId, SupportTicketId, SendTimestamp, ReadTimestamp, Text$) {
        super();
        this.SupportQuestionId = (SupportQuestionId | 0);
        this.SupportTicketId = (SupportTicketId | 0);
        this.SendTimestamp = SendTimestamp;
        this.ReadTimestamp = ReadTimestamp;
        this.Text = Text$;
    }
}

export function Question$reflection() {
    return record_type("MyStore.Domain.Support.Question", [], Question, () => [["SupportQuestionId", int32_type], ["SupportTicketId", int32_type], ["SendTimestamp", class_type("System.DateTimeOffset")], ["ReadTimestamp", option_type(class_type("System.DateTimeOffset"))], ["Text", string_type]]);
}

export function Question_ToDomain_Z758746DF(dto) {
    return new Question(dto.supportQuestionId, dto.supportTicketId, dto.sendTimestamp, map((x_9) => x_9, ofNullable(dto.readTimestamp)), dto.text);
}

export function Question__FromDomain(this$) {
    return new QuestionDto(this$.SupportQuestionId, this$.SupportTicketId, this$.SendTimestamp, toNullable(map((x_9) => x_9, this$.ReadTimestamp)), this$.Text);
}

export class Ticket extends Record {
    constructor(SupportTicketId, CustomerId, SupportOperatorId, OrderId, CreateTimestamp) {
        super();
        this.SupportTicketId = (SupportTicketId | 0);
        this.CustomerId = (CustomerId | 0);
        this.SupportOperatorId = (SupportOperatorId | 0);
        this.OrderId = OrderId;
        this.CreateTimestamp = CreateTimestamp;
    }
}

export function Ticket$reflection() {
    return record_type("MyStore.Domain.Support.Ticket", [], Ticket, () => [["SupportTicketId", int32_type], ["CustomerId", int32_type], ["SupportOperatorId", int32_type], ["OrderId", option_type(int32_type)], ["CreateTimestamp", class_type("System.DateTimeOffset")]]);
}

export function Ticket_ToDomain_5519A0CB(dto) {
    return new Ticket(dto.supportTicketId, dto.customerId, dto.supportOperatorId, map((x_9) => x_9, ofNullable(dto.orderId)), dto.createTimestamp);
}

export function Ticket__FromDomain(this$) {
    return new TicketDto(this$.SupportTicketId, this$.CustomerId, this$.SupportOperatorId, toNullable(map((x_9) => x_9, this$.OrderId)), this$.CreateTimestamp);
}

