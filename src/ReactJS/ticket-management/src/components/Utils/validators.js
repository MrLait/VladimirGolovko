import {messages} from "../Constants/validateExceptionMessages";

export const required = value => {
        if (value) return undefined;
    return 'Field is required';
}

export const priceValidation = value => {
    debugger;
    if (value > 0) return undefined;
    if (value === '0') return messages.PriceIsZero;
    if (value < 0) return messages.PriceIsNegative;
}

export const emailShouldContainАt = value => {
    if (!value) return undefined;
    const emailValid = value.match(/^([\w.%+-]+)@([\w-]+\.)+([\w]{2,})$/i);
    if (emailValid) return undefined;
    return 'Invalid mail format, example user@gmail.com';
}