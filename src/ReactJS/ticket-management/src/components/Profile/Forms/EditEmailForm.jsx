import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import {emailShouldContainАt} from "../../Utils/validators";
import style from "./Form.module.css"

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
                                   validate={[emailShouldContainАt]} component={Input}/>
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
    form: 'email'
})(EditEmailForm)
