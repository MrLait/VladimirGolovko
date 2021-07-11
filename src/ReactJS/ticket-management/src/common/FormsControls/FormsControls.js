import React from "react";
import styles from './FormsControls.module.css'
import {useTranslation} from "react-i18next";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const FormControl = ({input, meta, child, ...props}) => {
    const {t} = useTranslation();
    const hasError = meta.touched && meta.error;
    return (
        <div className={styles.formControl + " " + (hasError ? styles.error : "")}>
            {props.children}
            {hasError && <span> {t(meta.error)}</span>}
        </div>
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

// export const renderDatePicker = ({input, placeholder, defaultValue, meta: {touched, error} }) => (
//     <div>
//         <DatePicker {...input} dateForm="MM/DD/YYYY" selected={input.value ? moment(input.value, 'YYYY-MM-DD') : null} />
//         {touched && error && <span>{error}</span>}
//     </div>
// );


// export const renderDatePicker = ({input, meta, ...props}) => {
//     debugger;
//     const hasError = meta.touched && meta.error;
//     return (
//         <div className={styles.formControl + " " + (hasError ? styles.error : "")}>
//             <DatePicker {...input} selected = {input.value} {...props}/>
//             {hasError && <span>{meta.error}</span>}
//         </div>
//     )
// }

export const renderDatePicker = ({input, placeholder, defaultValue, meta: {touched, error} }) => (
    <div>
        <DatePicker {...input} dateForm="MM/DD/YYYY" selected={input.value ? (input.value) : null} />
        {touched && error && <span>{error}</span>}
    </div>
);