import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import style from "./Form.module.css"
import {formNames} from "../../Constants/formNames";

const EditSurnameForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td className={style.td}> {t('Surname')} - {props.surname} </td>
                    <td>
                        <span>
                            <Field placeholder={t("Edit surname")} name={"surname"}
                                   component={Input}/>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit surname')}
                            </button>
                        </span>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: formNames.Surname
})(EditSurnameForm)
