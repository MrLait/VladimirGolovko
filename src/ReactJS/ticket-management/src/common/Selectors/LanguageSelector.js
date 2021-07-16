import React from "react";
import i18n from "i18next";

const LanguageSelector = (props) => {
    const onChange = (currProps) => {
        props.editLanguage(currProps.target.value);
        i18n.changeLanguage(currProps.target.value);
    }
    return <select id={props.language} name={'language'} onChange={onChange} value={props.language}>
        <option value='en'>English</option>
        <option value='ru'>Русский</option>
        <option value='be'>Белорусский</option>
    </select>
}

export default LanguageSelector;