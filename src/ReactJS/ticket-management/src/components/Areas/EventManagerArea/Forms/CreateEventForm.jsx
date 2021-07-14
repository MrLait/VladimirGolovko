import React from "react";
import {useTranslation} from "react-i18next";
import {Field, reduxForm} from "redux-form";
import {priceValidation, required} from "../../../Utils/validators";
import {Input, renderDatePicker} from "../../../../common/FormsControls/FormsControls";
import {Redirect} from "react-router";
import {formNames} from "../../../Constants/formNames";
import {fieldNames, fieldTypes} from "../../../Constants/fieldConst";
import style from "../../../../common/FormsControls/FormsControls.module.css";

const CreateEventForm = (props) => {
    const {t} = useTranslation();
    return (
        <form onSubmit={props.handleSubmit}>
            <div>
                <Field placeholder={t("Name")} name={fieldNames.name} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field placeholder={t("Description")} name={fieldNames.description} validate={[required]} component={Input}/>
            </div>
            <div>
                <Field type={'number'} placeholder={t("LayoutId")} name={fieldNames.layoutId} validate={[required]}
                       component={Input}/>
            </div>
            <div>
                <Field placeholder={t("StartDateTime")} name={fieldNames.startDateTime} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("EndDateTime")} name={fieldNames.endDateTime} validate={[required]}
                       component={renderDatePicker}/>
            </div>
            <div>
                <Field placeholder={t("ImageUrl")} name={fieldNames.imageUrl} validate={[required]} component={Input}/>
            </div>

            <div>
                {
                    props.event && props.event.eventAreas.length > 0
                        ? props.event.eventAreas.map(ea =>
                            <div>
                                <Field type={fieldTypes.number} placeholder={t("Price")} name={`${fieldNames.eventAreas}[${ea.id}].${fieldNames.price}`}
                                       validate={[required, priceValidation ]}
                                       component={Input}/>
                            </div>
                        )
                        : ""
                }
            </div>
            <div>
                {props.error && <div className={style.formSummaryError}>{t(props.error)}</div>}

                {props.event && props.event.eventAreas
                    ?
                    <button disabled={props.inProgress}>
                        {t('PublishEvent')}
                    </button>
                    :
                    <button disabled={props.inProgress}>
                        {t('CreateEvent')}
                    </button>
                }
                {props.isCreateEventSuccessful
                    ? <Redirect to={'/eventManagerArea'}/>
                    : ""}
            </div>
        </form>
    )
}

export default reduxForm(
    {
        form: formNames.CreateEvent
    }
)(CreateEventForm)
