import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";

const EditSurnameForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td> {t('Surname')} - {props.surname} </td>
                    <td>
                        <div>
                            <Field placeholder={t("Edit surname")} name={"surname"}
                                   component={Input}/>
                        </div>
                    </td>
                    <td>
                        <div>
                            <button disabled={props.clickingInProgress}>
                                {t('Edit surname')}
                            </button>
                        </div>
                    </td>
                </tr>
            </table>
        </form>
    )
}

export default reduxForm({
    form: 'surname'
})(EditSurnameForm)
