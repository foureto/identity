﻿namespace g.identity.business.Handlers.Client.Models;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Code { get; set; }
    public bool RequiresSecondFactor { get; set; }
}