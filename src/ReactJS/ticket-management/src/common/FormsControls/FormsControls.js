import React from "react";
import styles from './FormsControls.module.css'
import {useTranslation} from "react-i18next";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const FormControl = ({input, meta, child, ...props}) => {
    const {t} = useTranslation();
    const hasError = meta.touched && meta.error;
    return (
        <span className={styles.formControl + " " + (hasError ? styles.error : "")}>
            {props.children}
            {hasError && <span> {t(meta.error)}</span>}
        </span>
    )
}

export const Textarea = (props) => {
    const {input, meta, child, ...restProps} = props;
    return <FormControl {...props}><textarea {...input}{...restProps}/> </FormControl>
}

export const Input = (props) => {
    const {input, meta, child, ...restProps} = props;
    return <FormControl {...props}><input {...input}{...restProps}/> </FormControl>
}

export const renderDatePicker = ({input, placeholder, defaultValue, meta: {touched, error} }) => (
    <div>
        <DatePicker {...input} dateForm="MM/DD/YYYY" selected={input.value ? (input.value) : null} />
        {touched && error && <span>{error}</span>}
    </div>
);