// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;

namespace Microsoft.Dafny;
public static class ErrorDetail {

  public enum ErrorID {
    None,

    p_duplicate_modifier,
    p_abstract_not_allowed,
    p_no_ghost_for_by_method,
    p_ghost_forbidden_default,
    p_ghost_forbidden,
    p_no_static,
    p_deprecated_attribute,
    p_literal_string_required,
    p_no_leading_underscore,
    p_bitvector_too_large,
    p_array_dimension_too_large,
    p_superfluous_export,
    p_bad_module_decl,
    p_extraneous_comma_in_export,
    p_top_level_field,
    p_bad_datatype_refinement,



    p_bad_const_initialize_op,
    p_deprecated_semicolon,
    sc_malformed_pragma,
    sc_unknown_pragma,
  }

  // This dictionary maps an errorID (error code) to a code action and explanatory text
  // (which could be moderately lengthy)


  static private Dictionary<ErrorID, string> errorDetailMap = new Dictionary<ErrorID, string>();

  static ErrorDetail() { init(); }

  static bool initialized = false;
  public static void init() {
    if (!initialized) {
      ParserErrorDetail.init();
      initialized = true;
    }
  }
  // Adds information into the dictionary
  public static void Add(ErrorID errorID, string detail) {
    errorDetailMap.Add(errorID, detail);
  }

  // Returns the error detail for the given error ID;
  // result could be null if there is no such detail
#nullable enable
  public static string? GetDetail(ErrorID errorID) {
    if (errorDetailMap.TryGetValue(errorID, out var result)) {
      return result;
    }
    return null;
  }
#nullable disable
}


