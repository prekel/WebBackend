import { Attribute } from "../.fable/fable-library.3.1.11/Types.js";
import { class_type } from "../.fable/fable-library.3.1.11/Reflection.js";

export class RequiredAttribute extends Attribute {
    constructor() {
        super();
    }
}

export function RequiredAttribute$reflection() {
    return class_type("Fable.System.ComponentModel.Annotations.RequiredAttribute", void 0, RequiredAttribute, class_type("System.Attribute"));
}

export function RequiredAttribute_$ctor() {
    return new RequiredAttribute();
}

