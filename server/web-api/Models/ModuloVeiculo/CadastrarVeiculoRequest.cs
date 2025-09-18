﻿namespace GestaoDeEstacionamento.WebApi.Models.ModuloVeiculo;

public record CadastrarVeiculoRequest(
    string Placa,
    string Modelo,
    string Cor,
    Guid HospedeId
    );

public record CadastrarVeiculoResponse(Guid Id);