import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import style from "./Form.module.css"
import {formNames} from "../../Constants/formNames";

const EditPasswordForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td className={style.password}>
                        <div>
                            <Field placeholder={t("Old password")} name={"oldPassword"}
                                   component={Input} type={'password'}/>
                            <Field placeholder={t("New password")} name={"newPassword"}
                                   component={Input} type={'password'}/>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit password')}
                            </button>
                        </div>
                    </td>
                    <div>
                        {props.isNewPassword
                        ? "done"
                        : ""}
                    </div>
                    {props.error && <div className={style.formSummaryError}>{t(props.error)}</div>}
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: formNames.EditPassword
})(EditPasswordForm)
