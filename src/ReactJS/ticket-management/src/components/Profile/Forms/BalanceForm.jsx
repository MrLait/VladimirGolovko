import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {Input} from "../../../common/FormsControls/FormsControls";

const BalanceForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <table>
                <tr>
                    <td> {t('Balance')} - {props.balance} {t('Currency')} </td>
                    <td>
                        <div>
                            <Field type="number" placeholder={t("Deposit")} name={"balance"}
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

export default reduxForm({
    form: 'balance'
})(BalanceForm)
