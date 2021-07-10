import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";

const EditFirstNameForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td> {t('FirstName')} - {props.firstName} </td>
                    <td>
                        <div>
                            <Field placeholder={t("Edit first name")} name={"firstName"}
                                   component={Input}/>
                        </div>
                    </td>
                    <td>
                        <div>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit first name')}
                            </button>
                        </div>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: 'firstName'
})(EditFirstNameForm)
