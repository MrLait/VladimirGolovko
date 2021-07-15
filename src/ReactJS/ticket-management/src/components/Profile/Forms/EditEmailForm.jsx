import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import {emailShouldContainAt} from "../../Utils/validators";
import style from "./Form.module.css"
import {formNames} from "../../Constants/formNames";

const EditEmailForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td className={style.td}> {t('Email')} - {props.email} </td>
                    <td>
                        <span>
                            <Field placeholder={t("Edit email")} name={"email"}
                                   validate={[emailShouldContainAt]} component={Input}/>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit email')}
                            </button>
                        </span>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: formNames.Email
})(EditEmailForm)
