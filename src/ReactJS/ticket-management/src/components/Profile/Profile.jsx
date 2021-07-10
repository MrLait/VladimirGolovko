import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {required} from "../Utils/validators";
import {Input} from "../../common/FormsControls/FormsControls";

let Profile = (props) => {
    const {t} = useTranslation();
    const onSubmit = (formData) => {
        console.log(formData);
        props.deposit(formData.balance);
    }
    return (
        <BalanceReduxForm {...props} onSubmit={onSubmit}/>
    )
}

const BalanceForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <dt> {t('Balance')} - {props.balance} {t('Currency')} </dt>
                    <td>
                        <div>
                            <Field placeholder={t("Deposit")} name={"balance"}
                                   component={Input}/>
                        </div>
                    </td>
                    <td>
                        <div>
                            <button disabled={props.clickingInProgress}>
                                {t('Deposit')}
                            </button>
                        </div>
                    </td>
                </tr>
            </table>
        </form>
    )
}

const BalanceReduxForm = reduxForm(
    {
        form: 'balance'
    }
)(BalanceForm)

export default Profile;