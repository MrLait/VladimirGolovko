import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";
import {emailShouldContainАt} from "../../Utils/validators";

const EditEmailForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td> {t('Email')} - {props.email} </td>
                    <td>
                        <div>
                            <Field placeholder={t("Edit email")} name={"email"}
                                   validate={[emailShouldContainАt]} component={Input}/>
                        </div>
                    </td>
                    <td>
                        <div>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit email')}
                            </button>
                        </div>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: 'email'
})(EditEmailForm)