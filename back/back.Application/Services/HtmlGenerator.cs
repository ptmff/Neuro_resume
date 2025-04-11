using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using back.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using back.Application.Interfaces;

namespace back.Application.Services;

public class HtmlGenerator : IHtmlGenerator
{
    public string Generate(ResumeDto resumeDto)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>{resumeDto.Title}</title>
    <style>
        body {{ font-family: Arial, sans-serif; background: #fff; color: #333; padding: 2rem; }}

        .resume-preview {{
            padding: 1.5rem;
            border-radius: 1rem;
            background: #f9f9f9;
            font-family: Arial, sans-serif;
            border: 2px solid transparent;
            position: relative;
        }}

        .template-классический {{ font-family: 'Times New Roman', Times, serif; }}
        .template-современный {{ background: white; }}
        .template-креативный {{ background: white; }}
        .template-минималистичный {{ background: white; border: 1px solid #e0e0e0; }}
        .template-профессиональный {{ background: white; }}
        .template-технический {{ background: white; color: #ccc; }}

        h1, h2, h3, h4 {{
            margin-bottom: 0.5rem;
        }}

        h3 {{
            font-size: 1.8rem;
            font-weight: bold;
        }}

        h4 {{
            font-size: 1.2rem;
            font-weight: 600;
        }}

        p {{
            margin: 0.25rem 0;
        }}

        ul {{
            padding-left: 1.2rem;
            margin: 0.5rem 0;
        }}

        li {{
            margin-bottom: 0.25rem;
        }}

        .timeline {{
            border-left: 2px solid #9d00ff;
            padding-left: 1rem;
            margin-bottom: 1rem;
        }}

        .timeline-entry {{
            position: relative;
            margin-bottom: 1.5rem;
        }}

        .timeline-entry .dot {{
            position: absolute;
            left: -0.75rem;
            top: 0.3rem;
            width: 0.6rem;
            height: 0.6rem;
            background: #9d00ff;
            border-radius: 50%;
        }}

        .timeline-entry .content {{
            margin-left: 0.5rem;
        }}

        .skill-chip {{
            display: inline-block;
            padding: 0.3rem 0.6rem;
            border-radius: 999px;
            font-size: 0.75rem;
            color: white;
            background: linear-gradient(135deg, #9d00ff, #00e5ff);
            margin: 0.2rem;
        }}
    </style>
</head>
<body class='resume-preview template-{resumeDto.Template?.ToLower()}'>
    <h1>{resumeDto.Title}</h1>
    <p><strong>Дата:</strong> {resumeDto.Date.ToShortDateString()}</p>
    <p><strong>Должность:</strong> {resumeDto.Job}</p>
    <p><strong>Город:</strong> {resumeDto.City}</p>
    <p><strong>Описание:</strong> {resumeDto.Description}</p>
    <h2>Навыки</h2>
    <ul>
        {string.Join(Environment.NewLine, resumeDto.Skills.Select(s => $"<li class='skill-chip'>{s}</li>"))}
    </ul>
    <h2>Опыт</h2>
    <div class='timeline'>
        {string.Join(Environment.NewLine, resumeDto.Experience.Select(exp =>
        $"<div class='timeline-entry'><div class='dot'></div><div class='content'><strong>{exp.Position}</strong> в {exp.Company}<br/><span>{exp.StartDate} - {exp.EndDate}</span><p>{exp.Description}</p></div></div>"))}
    </div>
</body>
</html>";
    }
}