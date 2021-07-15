import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import style from "./Form.module.css"

const EditFirstNameForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td className={style.td}> {t('FirstName')} - {props.firstName} </td>
                    <td>
                        <span>
                            <Field placeholder={t("Edit first name")} name={"firstName"}
                                   component={Input}/>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit first name')}
                            </button>
                        </span>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: 'firstName'
})(EditFirstNameForm)
